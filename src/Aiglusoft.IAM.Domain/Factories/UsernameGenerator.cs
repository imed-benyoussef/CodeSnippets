namespace Aiglusoft.IAM.Domain.Factories
{
    public static class UsernameGenerator
    {
        private static Random _random = new Random();

        public static async Task<string> GenerateUsernameAsync(string firstName, string lastName, Func<string, Task<bool>> isUsernameTakenAsync)
        {
            // Normalize the input
            firstName = firstName.Trim().Replace(" ","").ToLower();
            lastName = lastName.Trim().Replace(" ", "").ToLower();

            // Generate the base username based on a random choice
            string baseUsername = GenerateBaseUsername(firstName, lastName);
            string username = baseUsername;

            // Ensure the username is unique by appending a random suffix if necessary
            while (await isUsernameTakenAsync(username))
            {
                username = $"{baseUsername}{GenerateRandomSuffix(4)}";
            }

            return username;
        }

        private static string GenerateBaseUsername(string firstName, string lastName)
        {
            // Randomly choose to use first name, last name, or both
            int choice = _random.Next(3);
            return choice switch
            {
                0 => firstName,
                1 => lastName,
                _ => $"{firstName}.{lastName}"
            };
        }

        private static string GenerateRandomSuffix(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[_random.Next(chars.Length)];
            }

            return new string(stringChars);
        }
    }
}
