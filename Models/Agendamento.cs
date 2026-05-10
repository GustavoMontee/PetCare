using System.ComponentModel.DataAnnotations;

namespace PetCare.Models
{
    public class Agendamento
    {
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; }

        [Required]
        public int PetId { get; set; }

        public Pet? Pet { get; set; }

        [Required]
        public int ServicoId { get; set; }

        public Servico? Servico { get; set; }

        [Required]
        [Display(Name = "Data e Hora")]
        public DateTime DataHora { get; set; }

        public string? Observacoes { get; set; }

        public StatusAgendamento Status { get; set; }
    }
}