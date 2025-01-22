using ChatApp.Model;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Collections.Generic;

namespace ChatApp.Converters
{
    public class ParticipantNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var participants = value as List<AppUser>; // Assuming Participants is a List<AppUser>
            var currentUser = parameter as AppUser; // CurrentUser passed as parameter

            // Check if participants and currentUser are valid
            if (participants != null && currentUser != null)
            {
                // Check if there are at least two participants
                if (participants.Count >= 2)
                {
                    // Compare currentUser.UserName with each participant's username
                    if (participants[0].UserName == currentUser.UserName)
                    {
                        return participants[0].UserName;
                    }
                    else if (participants[1].UserName == currentUser.UserName)
                    {
                        return participants[1].UserName;
                    }
                }
            }

            // Return a fallback value if no match is found
            return "Unknown"; // Or any other fallback value
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
