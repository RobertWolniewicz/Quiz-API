using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuizAPI.UnitTests
{
    internal class CategoryTests
    {
        [Test]
        public void CategoryPost()
        {
            var client = new RestClient("https://localhost:7165");
            var request = new RestRequest("Category", Method.Post);
            request.AddJsonBody(new { Name = "Game of throne", CorrectAnswersPercent = 70 });
            var response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
        }
    }
}
