using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class Tree
    {
        public TreeNode currentNode;
        public TreeNode root;
        public List<bool?[]> traitsArray;
        public List<TreeNode> _tree;
        public Tree()
        {
            currentNode = null;
            root = CreatingFirstNode();
            traitsArray = CreateArray(root.animals);
            _tree = CreateTree();
        }

        public List<TreeNode> CreateTree()
        {
            var tree = CreatingTree(root);
            return tree;
        }
        
        
        public List<bool?[]> CreateArray(List<Animals> list)
        {
            List<bool?[]> traits = new List<bool?[]>();
            bool?[] temp = { null,null,null,null};
            foreach (Animals item in list) {
                bool?[] temp2 = { null, null, null, null };
                temp2[0] = item.hasFur;
                temp2[1] = item.isBird;
                temp2[2] = item.eatsFruit;
                temp2[3] = item.huntsRabbit;
                traits.Add(temp2);
            }
            return traits;
        }

        public Dictionary<int, string> CreateDictionary(List<Animals> animals)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            foreach (Animals item in animals)
            {
                dictionary.Add(item.Id, item.Animal1);
            }
            return dictionary;
        }

        private List<TreeNode> CreatingTree(TreeNode root)
        {
            int index = 0;
            List<TreeNode> treeNodes = new List<TreeNode>();
            treeNodes.Add(root);
            index = 1;

            while (index<10)
            {
                root = treeNodes.ElementAt(index-1);
                if(root.animals == null)
                {
                    index++;
                    continue;
                }

                if (root.animals.Count() == 1)
                {
                    TreeNode yesNode = new TreeNode();
                    TreeNode noNode = new TreeNode();
                    yesNode = CreatingLeaf(true, root);
                    noNode = CreatingLeaf(false, root);
                    treeNodes.Add(noNode);
                    treeNodes.Add(yesNode);

                }
                else
                {
                    TreeNode yesNode = new TreeNode();
                    yesNode = CreatingNode(true, root);
                    TreeNode noNode = new TreeNode();
                    noNode = CreatingNode(false, root);
                    treeNodes.Add(noNode);
                    treeNodes.Add(yesNode);
                }
                index++;

                foreach (TreeNode item in treeNodes)
                {   if (item.animals == null){
                        continue;
                    }
                    if(item.Id == index)
                    {
                        root = item;
                        break;
                    }
                }
            }

            return treeNodes;
        }

        private TreeNode CreatingNode(bool yesno, TreeNode previous)
        {
            var previousList = previous.animals;
            int index = previous.Id;
            TreeNode current = new TreeNode();
            current.animals = ChoosingForList(previousList, yesno);
            current.checkedQuestions = GetQuestionNumber(previous) + 1;
            current.Id = index * 2;
            if (yesno)
            {
                current.Id++;
            }
            return current;
        }

        private TreeNode CreatingLeaf(bool? yesno,TreeNode last)
        {
            var previousList = last.animals;
            int index = last.Id;
            int question = last.checkedQuestions;
            TreeNode leaf = new TreeNode();
            leaf.Id = index * 2;
            List<bool?[]> traitsArray = CreateArray(last.animals);
            if (traitsArray.First()[question] == yesno)
            {
                leaf.checkedQuestions = question++;
                leaf.Id++;
                leaf.animals = last.animals;
            }
            else
            {
                leaf.animals = null;

            }
            return leaf;
        }

        // ------------------------------------

        private int GetQuestionNumber(TreeNode previous)
        {
            int i = 0;
            var previousList = previous.animals;
            List<bool?[]> traitsArray = CreateArray(previous.animals);
            i = GetIndex(traitsArray);
            return i;
        }

       // ------------------------------------

        private List<Animals> ChoosingForList(List<Animals> previous, bool answer)
        {
            List<Animals> newList = new List<Animals>();
            if (previous == null)
            {
                return null;
            }
            List<bool?[]> traitsArray = CreateArray(previous);
            int index = 0;
            if (!traitsArray.Any())
            {
                return null;
            }

            int number = traitsArray.First().Count();
            index = GetIndex(traitsArray);

                if (index == number)
                {
                    return previous;
                }
            
            int traitIndex=0;
            foreach (bool?[] item in traitsArray)
            {
                if (item[index] == answer)
                {
                    newList.Add(previous.ElementAt(traitIndex));  
                }
               traitIndex++;
               
            }

            return newList;
        }

        private int GetIndex(List<bool?[]>list)
        {
            int index = 0;
            int number = list.First().Count();
            for (index = 0; index < number; index++)
            {
                bool? temp = list.First()[index];
                foreach (bool?[] item in list)
                {
                    if (temp != item[index])
                    {
                        return index;
                    }
                }
            }
            return number;
        }

        private List<Animals> RemoveFirst(List<Animals> previous)
        {
            previous.RemoveAt(0);
            return previous;
        }

        private TreeNode CreatingFirstNode()
        {
            TreeNode root = new TreeNode();
            List<Animals> fullList = new List<Animals>();
            fullList = CreateAnimalList();
            root.animals = fullList;
            root.Id = 1;
            root.checkedQuestions=0;
            return root;
        }

        private List<Animals> CreateAnimalList()
        {
            using (Database1Entities dc = new Database1Entities())
            {
                var animalList = dc.Animals1.OrderBy(a => a.Id).ToList();
                return animalList;
            }
        }
    }
    
}