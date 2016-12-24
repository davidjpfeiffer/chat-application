# Chat Application
A socket-based desktop chat application written in C#

### About

This project includes both a server and a client program. Once the server is started, clients will be able to connect to the server and both the client and server will be able to send messages to each other via long-lived socket connections. By default, the connection is established over localhost on port 2000 but the executables for both server and client take optional parameters that can override these defaults.

### Features

- Asynchronous communication between clients and server
- Supports multiple clients on a single server
- Clean and simple graphical user interface
- Gracefully handles error events like unexpected server shutdown
