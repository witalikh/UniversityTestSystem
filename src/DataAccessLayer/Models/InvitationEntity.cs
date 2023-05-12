using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Enums;

namespace DataAccessLayer.Models
{
    public class InvitationEntity
    {
        private string _email;

        [Key]
        public Guid uuid { get; set; }

        [Required]
        public string Email
        {
            get => this._email;
            set => this._email = value;
        }

        [Required]
        [ForeignKey("ClassroomEntity")]
        public int ClassroomId { get; set; }

        public ClassroomEntity ClassroomEntity { get; set; }

        [ForeignKey("UserEntity")]
        public string? UserId { get; set; }

        public UserEntity? User { get; set; }

        public InvitationStatus InvitationStatus { get; set; }
    }
}
