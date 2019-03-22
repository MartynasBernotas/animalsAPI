using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
using RestSharp;
using System.Web.Http.Cors;
using System.Web.Cors;


namespace WebAPI
{
    public class TreeNode
    {
        public int Id { get; set; }
        public List<Animals> animals;
        public int checkedQuestions;
    }
}