using System.Collections.Generic;

namespace PetCare.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        public string Nome { get; set; } = "";
        public string Email { get; set; } = "";
        public string Telefone { get; set; } = "";
        public string Endereco { get; set; } = "";

        public ICollection<Pet> Pets { get; set; } = new List<Pet>();
    }
}