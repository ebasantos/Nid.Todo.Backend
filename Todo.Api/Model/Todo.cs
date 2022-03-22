namespace Todo.Api.Model
{
    public class Todo
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Concluido { get; set; } = false;
    }
}
