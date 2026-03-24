using System;
using System.Collections.Generic;
using System.Linq;

namespace Riddle_Shutova_PRI122.Managers
{
    public class JumpManager
    {
        private Dictionary<string, float> characterOffsets;
        private List<JumpSequence> activeSequences;
        private List<string> jumpOrder;
        private float jumpHeight = 2.2f;
        private float jumpSpeed = 0.08f;
        private int maxMultiplier = 1; 
        private float multiplierIncrement = 0.2f;
        private Dictionary<string, float> jumpDurationMultipliers;

        public event EventHandler JumpUpdated;

        private class JumpSequence
        {
            public int SequenceId { get; set; }
            public string CurrentCharacter { get; set; } = "Puppy";
            public bool IsActive { get; set; } = true;
            public float Multiplier { get; set; } = 1.0f; 
            public Dictionary<string, JumpState> CharacterStates { get; set; } = new Dictionary<string, JumpState>();
            public float JumpSpeed { get; set; } = 0.09f;
        }

        private class JumpState
        {
            public float Offset { get; set; } = 0f;
            public bool IsJumping { get; set; } = false;
            public bool IsAscending { get; set; } = true;
            public float TargetHeight { get; set; } = 0f;
            public float SpeedMultiplier { get; set; } = 1.0f;
        }

        public JumpManager()
        {
            characterOffsets = new Dictionary<string, float>
            {
                { "Puppy", 0f },
                { "Dog", 0f },
                { "Daughter", 0f },
                { "Dad", 0f }
            };

            jumpDurationMultipliers = new Dictionary<string, float>
        {
            { "Puppy", 1.0f },     
            { "Dog", 1.3f },       
            { "Daughter", 1.0f },  
            { "Dad", 1.0f }        
        };

            jumpOrder = new List<string> { "Puppy", "Dog", "Daughter", "Dad" };
            activeSequences = new List<JumpSequence>();
        }

        public void StartJumpSequence(bool isDoubleJump = false)
        {
            var activeSequence = activeSequences.FirstOrDefault(s => s.IsActive && s.CurrentCharacter == "Puppy");

            if (activeSequence != null && isDoubleJump)
            {
                if (activeSequence.Multiplier < maxMultiplier)
                {
                    activeSequence.Multiplier += multiplierIncrement;
                    foreach (var character in jumpOrder)
                    {
                        if (activeSequence.CharacterStates.ContainsKey(character))
                        {
                            var state = activeSequence.CharacterStates[character];
                            state.TargetHeight = jumpHeight * activeSequence.Multiplier;

                            if (state.IsJumping && state.IsAscending)
                            {
                                float speedFactor = jumpDurationMultipliers.ContainsKey(character) ?
                                                  jumpDurationMultipliers[character] : 1.0f;
                                state.Offset += jumpSpeed * multiplierIncrement * 2 * speedFactor;
                            }
                        }
                    }
                }
                return;
            }

            int newSequenceId = activeSequences.Count > 0 ? activeSequences.Max(s => s.SequenceId) + 1 : 1;
            var newSequence = new JumpSequence
            {
                SequenceId = newSequenceId,
                CurrentCharacter = "Puppy",
                IsActive = true,
                Multiplier = 1.0f,
                JumpSpeed = jumpSpeed
            };

            foreach (var character in jumpOrder)
            {
                newSequence.CharacterStates[character] = new JumpState
                {
                    TargetHeight = jumpHeight * newSequence.Multiplier,
                    SpeedMultiplier = jumpDurationMultipliers.ContainsKey(character) ?
                                    jumpDurationMultipliers[character] : 1.0f
                };
            }

            activeSequences.Add(newSequence);
        }

        public void UpdateJump()
        {
            foreach (var character in characterOffsets.Keys.ToList())
            {
                characterOffsets[character] = 0f;
            }

            bool anyActive = false;

            for (int i = 0; i < activeSequences.Count; i++)
            {
                var sequence = activeSequences[i];
                if (!sequence.IsActive) continue;

                anyActive = true;
                string currentCharacter = sequence.CurrentCharacter;
                var state = sequence.CharacterStates[currentCharacter];

                if (!state.IsJumping)
                {
                    state.IsJumping = true;
                    state.IsAscending = true;
                    state.Offset = 0f;
                    state.TargetHeight = jumpHeight * sequence.Multiplier;
                }

                if (state.IsJumping)
                {
                    if (state.IsAscending)
                    {
                        float speedMultiplier = 1.0f + (sequence.Multiplier - 1.0f) * 0.3f;

                        float characterSpeedMultiplier = state.SpeedMultiplier;

                        state.Offset += (jumpSpeed / characterSpeedMultiplier) * speedMultiplier;

                        if (state.Offset >= state.TargetHeight)
                        {
                            state.Offset = state.TargetHeight;
                            state.IsAscending = false;
                        }
                    }
                    else
                    {
                        float characterSpeedMultiplier = state.SpeedMultiplier;
                        state.Offset -= (jumpSpeed / characterSpeedMultiplier);

                        if (state.Offset <= 0f)
                        {
                            state.Offset = 0f;
                            state.IsJumping = false;

                            int currentIndex = jumpOrder.IndexOf(currentCharacter);
                            if (currentIndex + 1 < jumpOrder.Count)
                            {
                                sequence.CurrentCharacter = jumpOrder[currentIndex + 1];
                            }
                            else
                            {
                                sequence.IsActive = false;
                            }
                        }
                    }
                }

                foreach (var character in jumpOrder)
                {
                    if (sequence.CharacterStates.ContainsKey(character))
                    {
                        characterOffsets[character] += sequence.CharacterStates[character].Offset;
                    }
                }
            }

            activeSequences.RemoveAll(s => !s.IsActive);

            if (anyActive)
            {
                JumpUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public float GetCharacterOffset(string characterName)
        {
            if (characterOffsets.ContainsKey(characterName))
            {
                return characterOffsets[characterName];
            }
            return 0f;
        }

        public bool HasActiveSequences()
        {
            return activeSequences.Any(s => s.IsActive);
        }

        public bool IsPuppyInAir()
        {
            foreach (var sequence in activeSequences.Where(s => s.IsActive))
            {
                if (sequence.CharacterStates.ContainsKey("Puppy"))
                {
                    var state = sequence.CharacterStates["Puppy"];
                    if (state.IsJumping && state.Offset > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}