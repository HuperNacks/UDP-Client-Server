using UDP.Client;

ExpressEncription.RSAEncription.MakeKey(@"D:\Local\Task\UDP.Client\public.key", @"D:\Local\Task\UDP.Client\private.key");
UDPManager client = new UDPManager();

client.Start();
