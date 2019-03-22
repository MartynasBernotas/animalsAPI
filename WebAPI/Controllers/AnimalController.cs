using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
using RestSharp;
using System.Web.Http.Cors;
using System.Web.Cors;
using System.Reflection;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    public class AnimalController : ApiController
    {
        const string baseUrl = "http://localhost:51275/api/";
        Tree newTree;
        List<TreeNode> nodesList;
        Dictionary<int, string> traitsDictionary;

        public AnimalController()
        {
            traitsDictionary = newDictionary();

            
        }

        [HttpGet]
        [Route("api/animal/start")]
        public string Start(int index ){
            var _client = new RestClient(baseUrl);
            index = 0;
            string str = GetTrait(index);
            RestRequest request = new RestRequest("getTree", Method.GET);
            IRestResponse response = _client.Execute(request);
            return str;
        }

        [HttpGet]
        [Route("api/animal/yes")]
        public string AnswerYes(int index)
        {
            Tree newTree = new Tree();
            nodesList = newTree._tree;
            var _client = new RestClient(baseUrl);
            int check = nodesList.Count();
            string name = "noanimal";
            if (index > check)
            {
                return name;
            }
            if (nodesList.First(a => a.Id == index).animals != null)
            {
                name = nodesList.First(a => a.Id == index).animals.First().Animal1;
            }
            RestRequest request = new RestRequest("getTree", Method.GET);
            IRestResponse response = _client.Execute(request);
            return name;
        }

        [HttpGet]
        [Route("api/animal/no")]
        public string AnswerNo(int index)
        {
            Tree newTree = new Tree();
            nodesList = newTree._tree;
            var _client = new RestClient(baseUrl);
            int check = nodesList.Count();
            string trait = "noanimal";
            if (index > check)
            {
                return trait;
            }
            if (nodesList.First(a => a.Id == index).animals != null)
            {
                var temp = nodesList.First(a => a.Id == index);
                trait = GetTrait(temp.checkedQuestions+1);
            }

            
            RestRequest request = new RestRequest("getTree", Method.GET);
            IRestResponse response = _client.Execute(request);
            return trait;
        }

        [HttpGet]
        [Route("api/animal/nono")]
        public string AnswerNext(int index)
        {
            Tree newTree = new Tree();
            nodesList = newTree._tree;
            var _client = new RestClient(baseUrl);
            int check = nodesList.Count();
            string trait = "noanimal";
            if (index > check)
            {
                return "noanimal";
            }
            if (nodesList.First(a => a.Id == index).animals != null)
            {
                var temp = nodesList.First(a => a.Id == index);
                trait = GetTrait(temp.checkedQuestions);
            }

            
            RestRequest request = new RestRequest("getTree", Method.GET);
            IRestResponse response = _client.Execute(request);
            return trait;
        }

        public string GetTrait(int index)
        {
            index = index +2-1;
            string str = "noanimal";
            if (index < 2)
            {
                index = 2;
            }
            if (traitsDictionary.ContainsKey(index))
            {
                string value = traitsDictionary[index];
                switch (value)
                {
                    case ("hasFur"):
                        str = "has fur";
                        break;
                    case ("isBird"):
                        str = "has wings";
                            break;
                    case ("eatsFruit"):
                        str = "eat fruits";
                        break;
                    case ("huntsRabbit"):
                        str = "like to hunt rabbits";
                        break;
                }
            }


            return str;
        }
        [HttpGet]
        [Route("api/animal/tree")]
        public List<TreeNode> GetNodes()
        {
            Tree newTree = new Tree();
            var _client = new RestClient(baseUrl);
            List<TreeNode> trippy = newTree._tree;
            RestRequest request = new RestRequest("getTree", Method.GET);
            IRestResponse response = _client.Execute(request);
            return trippy;
        }

        [HttpPost]
        [Route("api/animal/add")]
        public IHttpActionResult PostData([FromBody] string ani)
        {
            var _client = new RestClient(baseUrl);
            RestRequest request = new RestRequest("add", Method.POST);
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.RequestFormat = DataFormat.Json;
            var result = JsonConvert.DeserializeObject<Animals>(ani);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (Database1Entities dc = new Database1Entities())
            {
                int number = dc.Animals1.Count();
                result.Id = number + 1;
                dc.Animals1.Add(result);
                dc.SaveChanges();
            }
            return Ok(ani);

        }

        public Dictionary<int, string> newDictionary()
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            Animals an = new Animals();
            Type t = an.GetType();
            int i = 0;
            PropertyInfo[] props = t.GetProperties();
            foreach(PropertyInfo prp in props)
            {
                dictionary.Add(i, prp.Name);
                i++;
            }
            return dictionary;
        }

    }
}
