using FuzzyExpert.Application.Common.Interfaces;

namespace FuzzyExpert.Application.Common.Implementations
{
    public class UniqueNameProvider : INameProvider
    {
        private char _availableCharacter = 'A';
        private ushort _availableNumber = 1;

        public string GetName()
        {
            string newName = $"{_availableCharacter}{_availableNumber}";
            UpdateAvailableInfo();
            return newName;
        }

        private void UpdateAvailableInfo()
        {
            if (_availableNumber == 9)
            {
                _availableNumber = 1;
                _availableCharacter++;
            }
            else
            {
                _availableNumber++;
            }
        }
    }
}