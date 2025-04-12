namespace EquipmentLoanApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public long Cpf { get; set; }  // FK para User
        public int ProductId { get; set; } // FK para Product
        public int Quantity { get; set; }

        // Navegação (opcional)
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
