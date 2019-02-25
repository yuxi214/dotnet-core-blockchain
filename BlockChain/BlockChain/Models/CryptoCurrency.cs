﻿using Newtonsoft.Json;
using RSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Models
{

   /**
    * Used to define CryptoCurrency
    * 
    * @author Davain Pablo Edwards
    * @license MIT 
    * @version 1.0
    */
    public class CryptoCurrency
    {

        // Initial current transactions list 
        private List<Transaction> _currentTransactions = new List<Transaction>();

        // Initial blockchain list 
        private List<Block> _chain = new List<Block>();

        // Initial of node list 
        private List<Node> _nodes = new List<Node>();

        private Block _lastBlock => _chain.Last();
        
        public string NodeId { get; private set; }
        static int blockCount = 0;
        static decimal reward = 1; // Set test reward value

        static string minerPrivateKey = "";
        static Wallet _minersWallet = RSA.RSA.KeyGenerate();

        /*
         * Constructor 
         */ 
        public CryptoCurrency()
        {
            minerPrivateKey = _minersWallet.PrivateKey;
            NodeId = _minersWallet.PublicKey;

            // Initial transaction
            var transaction = new Transaction { Sender = "0", Recipient = NodeId, Amount = 49, Fees = 0, Signature = "" };
            _currentTransactions.Add(transaction);

            CreateNewBlock(proof: 100, previousHash: "1"); // Genesis block 
        }

        /*
         * CreateNewBlock() Method to create new block 
         * 
         * @return block
         */
        private Block CreateNewBlock(int proof, string previousHash = null)
        {
            var block = new Block
            {
                Index = _chain.Count,
                TimeStamp = DateTime.Now,
                Transactions = _currentTransactions.ToList(),
                Proof = proof,
                PreviousHash = previousHash ?? GetHash(_chain.Last())
            };

            _currentTransactions.Clear();
            _chain.Add(block);
            return block;
        }

        /*
         * GetHash() Method to get block hashes 
         * 
         * @return blockText
         */ 
        private string GetHash(Block block)
        {
            string blockText = JsonConvert.SerializeObject(block);
            return GetSha256(blockText);
        }

        /*
         * GetSha256() Method to get SHA (Secure Hash Algorithm ) hash
         */ 
        private string GetSha256(string blockText)
        {
            var sha256 = new SHA256Managed();
            var hasBuilder = new StringBuilder();

            byte[] bytes = Encoding.Unicode.GetBytes(blockText);
            byte[] hash = sha256.ComputeHash(bytes);

            foreach (byte x in hash)
                hasBuilder.Append($"{x:x2}");

            return hasBuilder.ToString();
        }
    }
}