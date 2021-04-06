using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class EmailTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TemplateID { get; set; }

        [DisplayName("Template Name")]
        [Column(TypeName = "varchar(150)")]
        public string TemplateName { get; set; }

        [DisplayName("Email Subject")]
        [Column(TypeName = "varchar(150)")]
        public string SubjectContent { get; set; }

        [DisplayName("Email Body")]
        [Column(TypeName = "text")]
        public string TemplateContent { get; set; }

        [DisplayName("Email To")]
        [Column(TypeName = "varchar(150)")]
        public string EmailTo { get; set; }

        [DisplayName("Email CC")]
        [Column(TypeName = "varchar(150)")]
        public string EmailCC { get; set; }
    }
}