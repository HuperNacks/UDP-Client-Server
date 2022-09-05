using UDP.Client;

ExpressEncription.RSAEncription.MakeKey(@"D:\programare\Max volosenko\Task\UDP.Client\public.key", @"D:\programare\Max volosenko\Task\UDP.Client\private.key");
UDPManager client = new UDPManager();

client.Start();
