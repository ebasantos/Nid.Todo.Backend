namespace Todo.Api.Model
{
    public class UpdateTodo
    {
        public string Descricao { get; set; }
        public bool? Concluido { get; set; } = false;
    }
}
