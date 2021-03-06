﻿// Copyright (c) 2008-2017, Hazelcast, Inc. All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Hazelcast.Client;
using Hazelcast.Config;
using Hazelcast.Transaction;

namespace Hazelcast.Examples.Transactions
{
    internal class SimpleTransactionExample
    {
        private static void Run(string[] args)
        {
            Environment.SetEnvironmentVariable("hazelcast.logging.level", "info");
            Environment.SetEnvironmentVariable("hazelcast.logging.type", "console");

            var config = new ClientConfig();
            config.GetNetworkConfig().AddAddress("127.0.0.1");
            var client = HazelcastClient.NewHazelcastClient(config);

            var options = new TransactionOptions();
            options.SetTransactionType(TransactionOptions.TransactionType.OnePhase);
            var ctx = client.NewTransactionContext(options);
            ctx.BeginTransaction();
            try
            {
                var txMap = ctx.GetMap<string, string>("txn-map");
                txMap.Put("foo", "bar");
                ctx.CommitTransaction();
            }
            catch
            {
                ctx.RollbackTransaction();
            }

            var map = client.GetMap<string, string>("txn-map");
            Console.WriteLine("Value of foo is " + map.Get("key"));
            map.Destroy();
            client.Shutdown();
        }
    }
}