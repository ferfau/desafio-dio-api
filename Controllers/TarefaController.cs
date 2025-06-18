using Microsoft.AspNetCore.Mvc; 
using TrilhaApiDesafio.Context; 
using TrilhaApiDesafio.Models; 

namespace TrilhaApiDesafio.Controllers // Declara o namespace para os controladores.
{
    [ApiController] // Atributo que indica que esta classe é um controlador de API.
    [Route("[controller]")] // Atributo que define a rota base para este controlador (ex: /Tarefa).
    public class TarefaController : ControllerBase // Declara a classe do controlador, herdando de ControllerBase para funcionalidades de API.
    {
        private readonly OrganizadorContext _context; // Declara um campo privado e somente leitura para o contexto do banco de dados.

        public TarefaController(OrganizadorContext context) // Construtor da classe, injetando o contexto do banco de dados.
        {
            _context = context; // Atribui o contexto injetado ao campo privado.
        }

        [HttpGet("{id}")] // Atributo para um endpoint HTTP GET que aceita um ID na rota (ex: /Tarefa/1).
        public IActionResult ObterPorId(int id) // Método para obter uma tarefa por ID.
        {
            var tarefa = _context.Tarefas.Where(x => x.Id == id); // Busca tarefas no banco de dados onde o ID corresponde ao fornecido.

            if (tarefa == null) // Verifica se nenhuma tarefa foi encontrada.
                return NotFound(); // Retorna um status HTTP 404 (Não Encontrado).

            return Ok(); // Retorna um status HTTP 200 (OK).
        }

        [HttpGet("ObterTodos")] // Atributo para um endpoint HTTP GET com a rota específica "ObterTodos" (ex: /Tarefa/ObterTodos).
        public IActionResult ObterTodos() // Método para obter todas as tarefas.
        {
            var tarefas = _context.Tarefas.ToList(); // Obtém todas as tarefas do banco de dados e as converte para uma lista.

            return Ok(); // Retorna um status HTTP 200 (OK).
        }

        [HttpGet("ObterPorTitulo")] // Atributo para um endpoint HTTP GET com a rota específica "ObterPorTitulo".
        public IActionResult ObterPorTitulo(string titulo) // Método para obter tarefas por título.
        {
            var tituloTarefa = _context.Tarefas.Where(x => x.Titulo == titulo); // Busca tarefas onde o título corresponde ao fornecido.

            return Ok(tituloTarefa); // Retorna um status HTTP 200 (OK) com as tarefas encontradas.
        }

        [HttpGet("ObterPorData")] // Atributo para um endpoint HTTP GET com a rota específica "ObterPorData".
        public IActionResult ObterPorData(DateTime data) // Método para obter tarefas por data.
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date); // Busca tarefas onde a data corresponde à data fornecida (ignorando a hora).
            return Ok(tarefa); // Retorna um status HTTP 200 (OK) com as tarefas encontradas.
        }

        [HttpGet("ObterPorStatus")] // Atributo para um endpoint HTTP GET com a rota específica "ObterPorStatus".
        public IActionResult ObterPorStatus(EnumStatusTarefa status) // Método para obter tarefas por status.
        {
            var statusTarefa = _context.Tarefas.Where(x => x.Status == status); // Busca tarefas onde o status corresponde ao fornecido.
            return Ok(statusTarefa); // Retorna um status HTTP 200 (OK) com as tarefas encontradas.
        }

        [HttpPost] // Atributo para um endpoint HTTP POST (para criar um novo recurso).
        public IActionResult Criar(Tarefa tarefa) // Método para criar uma nova tarefa.
        {
            if (tarefa.Data == DateTime.MinValue) // Verifica se a data da tarefa é inválida (valor mínimo padrão).
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" }); // Retorna um status HTTP 400 (Requisição Inválida) com uma mensagem de erro.

            _context.Add(tarefa); // Adiciona a nova tarefa ao contexto do banco de dados (ainda não salva).
            _context.SaveChanges(); // Salva as alterações no banco de dados.

            // Retorna um status HTTP 201 (Criado) e um cabeçalho de localização para a nova tarefa.
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")] // Atributo para um endpoint HTTP PUT que aceita um ID na rota (para atualizar um recurso).
        public IActionResult Atualizar(int id, Tarefa tarefa) // Método para atualizar uma tarefa existente.
        {
            var tarefaBanco = _context.Tarefas.Find(id); // Busca a tarefa no banco de dados pelo ID.

            if (tarefaBanco == null) // Verifica se a tarefa não foi encontrada.
                return NotFound(); // Retorna um status HTTP 404 (Não Encontrado).

            if (tarefa.Data == DateTime.MinValue) // Verifica se a data fornecida para atualização é inválida.
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" }); // Retorna um status HTTP 400 (Requisição Inválida) com uma mensagem de erro.

            tarefaBanco.Descricao = tarefa.Descricao; // Atualiza a descrição da tarefa existente com a nova descrição.
            tarefaBanco.Titulo = tarefa.Titulo; // Atualiza o título da tarefa existente com o novo título.
            tarefaBanco.Status = tarefa.Status; // Atualiza o status da tarefa existente com o novo status.
            tarefaBanco.Titulo = tarefa.Titulo; // Esta linha parece ser um duplicata da atualização do título.

            _context.Update(tarefaBanco); // Marca a tarefa como modificada no contexto do banco de dados.

            _context.SaveChanges(); // Salva as alterações no banco de dados.

            return Ok(tarefaBanco); // Retorna um status HTTP 200 (OK) com a tarefa atualizada.
        }

        [HttpDelete("{id}")] // Atributo para um endpoint HTTP DELETE que aceita um ID na rota (para deletar um recurso).
        public IActionResult Deletar(int id) // Método para deletar uma tarefa.
        {
            var tarefaBanco = _context.Tarefas.Find(id); // Busca a tarefa no banco de dados pelo ID.

            if (tarefaBanco == null) // Verifica se a tarefa não foi encontrada.
                return NotFound(); // Retorna um status HTTP 404 (Não Encontrado).

            _context.Tarefas.Remove(tarefaBanco); // Remove a tarefa do contexto do banco de dados (ainda não salva).

            _context.SaveChanges(); // Salva as alterações no banco de dados.

            return NoContent(); // Retorna um status HTTP 204 (Sem Conteúdo), indicando sucesso na exclusão.
        }
    }
}