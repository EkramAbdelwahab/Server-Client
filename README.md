security project 



in our project,we want to make a single server multiclients project in c#,
the project aims to create a secure chat between the server and clients.
Firstly the server side:
to create a server  we use TcpListener class that provides simple methods that listen for  incoming connection requests(clients requests) in blocking synchronous mode. 
here we use a TcpClient to connect with a TcpListener(server) with an IP and port number.

TcpListener listener = new TcpListener(IPAddress.Any, portNum);

we created a TcpListener using an IP, a Local IP address and port number. Specify Any for the local IP address and  the local port number ...
then we used the (Start) method to begin listening for incoming connection requests from clients,Use AcceptTcpClient to pull a connection from the incoming connection request queue.


we used the ClientHandler class to hendle the clients connection requests,



the ClientHandler class has the start method  that has the threads that is responsible for receiving/handling  multible clients ,


and the process function that sends the public key and read encrepted  symmetric key from client,


the Decryption class that has the implementation of the encrypt and decrypt functions  to the data that will be send and received from/to the server and clients.



Secondly the client side:
The client side has the following form that before the client can send a message to the server by clicking the encrypt button… client have to load the public key  by clicking the loadPublicKey button
And then enter ,message will be sent to the server 

The server will receive the client message and decrypt it and then reply to the specific client ..
After the client receive the server reply message ,client can not see the message until clicking the show button 

In client side,we created  an object from the TcpClient

    TcpClient  tcpClient = new TcpClient();
And then calling the “connect” function that connect to the server,here we use localhost.
And create network stream that will carry the messages between clients and server.

In the client side .. we created the Encryption class that have the  encrypt and decrypt methods
 

                               Thx to Prof.Yousef  
  
