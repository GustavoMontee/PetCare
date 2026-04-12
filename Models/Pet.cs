using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PetCare.Models
{
    public class Pet
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Especie { get; set; }

        public string Raca { get; set; }

        public int? Idade { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Selecione um cliente válido")]
        public int ClienteId { get; set; }

        [ValidateNever]
        public Cliente Cliente { get; set; }
    }
}

