using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.Models
{
    public class Servico
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o nome do serviço")]
        [StringLength(100)]
        public string Nome { get; set; }

        [StringLength(500)]
        public string? Descricao { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

        public bool Ativo { get; set; } = true;
    }
}