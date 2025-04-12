namespace EquipmentLoanApi.Models
{
    public class User{
        public string Name { get; set; }
        public long Cpf { get; set; } // Usando long para representar o CPF
        public string Role { get; set; }
        public bool IsConsumer { get; set; }
    }
}
