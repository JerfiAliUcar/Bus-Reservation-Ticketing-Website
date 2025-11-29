using System.ComponentModel.DataAnnotations;

namespace Bus_Reservation_Ticketing_Website.Data.Entity
{
    public class ContactMessage
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [MaxLength(100)]
        public string Email { get; set; } 
        
        [Required(ErrorMessage = "Konu zorunludur")]
        [MaxLength(200)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Mesaj zorunludur")]
        public string Message { get; set; }

        public DateTime SentDate { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;
    }
}
