using DataAccessLayer.Models;

namespace BusinessLayer.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EducationPlace { get; set; }

        public UserViewModel() {}

        public UserViewModel(UserEntity user)
        {
            this.Id = user.Id;

            this.Email = user.Email;

            this.FirstName = user.FirstName;

            this.LastName = user.LastName;

            this.EducationPlace = user.EducationPlace;
        }
    }
}
