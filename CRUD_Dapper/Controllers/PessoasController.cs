using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using CRUD_Dapper.Models;
using System.Threading.Tasks;
using System.Data;
using Npgsql;
using Dapper;

namespace CRUD_Dapper.Controllers
{
    public class PessoasController : Controller
    {
        private readonly string ConnectionString = "User ID=postgres;Password=admin;Host=localhost;Port=5432;Database=PessoasDB;";
        public IActionResult Index()
        {
            IDbConnection con;
            try
            {
                string selecaoQuery = "SELECT * FROM pessoas";
                con = new NpgsqlConnection(ConnectionString);
                con.Open();
                IEnumerable<Pessoas> listaPessoas = con.Query<Pessoas>(selecaoQuery).ToList();
                return View(listaPessoas);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Pessoas pessoas)
        {
            if (ModelState.IsValid)
            {
                IDbConnection con;

                try
                {
                    string insersaoQuery = "INSERT INTO pessoas(nome, telefone, cpf, cep, endereco, bairro, cidade, estado) VALUES (@Nome, @Telefone, @CPF, @CEP, @Endereco, @Bairro, @Cidade, @Estado)";
                    con = new NpgsqlConnection(ConnectionString);
                    con.Open();
                    con.Execute(insersaoQuery, pessoas);
                    con.Close();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return View(pessoas);
        }

        [HttpGet]
        public IActionResult Edit(int pessoaid)
        {
            IDbConnection con;
            try
            {
                string selecaoQuery = "SELECT * FROM pessoas WHERE pessoaId = @pessoaId";
                con = new NpgsqlConnection(ConnectionString);
                con.Open();
                Pessoas pessoas = con.Query<Pessoas>(selecaoQuery, new { pessoasid = pessoaid }).FirstOrDefault();
                con.Close();

                return View(pessoas);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public IActionResult Edit(int pessoaid, Pessoas pessoas)
        {
            if (pessoaid != pessoas.PessoaId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                IDbConnection con;

                try
                {
                    con = new NpgsqlConnection(ConnectionString);
                    string atualizarQuery = "UPDATE pessoas SET Nome=@Nome, Telefone=@Telefone, CPF=@CPF, CEP=@CEP, Endereco=@Endereco, Bairro=@Bairro, Cidade=@Cidade, Estado=@Estado WHERE PessoaId=@PessoaId";
                    con.Open();
                    con.Execute(atualizarQuery, pessoas);
                    con.Close();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return View(pessoas);
        }

        [HttpPost]
        public IActionResult Delete(int pessoaid)
        {
            IDbConnection con;

            try
            {
                string excluirQuery = "DELETE FROM pessoas WHERE PessoaId=@PessoaId";
                con = new NpgsqlConnection(ConnectionString);
                con.Open();
                con.Execute(excluirQuery, new { PessoaId = pessoaid });
                con.Close();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
