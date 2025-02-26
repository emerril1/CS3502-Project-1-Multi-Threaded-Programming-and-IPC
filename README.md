# CS3502-Project-1-Multi-Threaded-Programming-and-IPC
Source code for Project A and batch command for Project B

Project A:

Project A oversees the implementation of a multi-threading application that handles threads, deadlocks, and synchronization.
Given is the source code for Project A written in C# in the Visual Studio Code IDE. The code is simplified as a banking application
that handles transfers between two accounts, as well as withrdawing and depositing to each of them. There is no user input, and
testing was done by hard coding chosen methods and fields into the main method.

Steps I took to get the program up and running:
1. Launch any chosen IDE
2. Make sure to have the .NET runtime
3. Create a .NET project that is able to compile C#
4. Create Deposit, Withdraw, and Transfer methods one by one in separate BankAccount class
5. Keep track of which account is being modified through IDs
6. Implement threading safety and deadlock handling
7. Create a driver class meant for testing the methods
8. Start at least 10 threads, that all either call Deposit, Withdraw, or Transfer
10. Change any hardcoded test data for the methods as needed
11. Wait till threads end processing
12. End program

To install and run:
1. Download ProjectA.cs
2. Open ProjectA.cs with any IDE with .NET and #C dependencies
3. Run and Test to your liking

Project B:

Project B oversees the implementation of IPC, or Inter-Process Communication. This can be shown by demonstrating pipes and how
they function using typical Linux commands and utilites. Given is the command used in the terminal of Ubuntu to demonstrate this 
project. The command copies the contents of one text file, which contains the Declaration of Indepedence, and puts in onto a new
file. The pipe process filters out the word "error" that was scattered throughout the original text file. The command also uses 
the pipe viewer (pv) utility, which tracks the ETA, size, and progress of the process. There is also error handling for any problems
with the pipe or the original text file.

Steps I took to create the command:
1. Launch the command terminal
2. Try out different piping processes such as 'cat' and 'tee'
3. Slowly build up the command by introducing error handling
4. Slow down the transfer of data through the pipe
5. Show real time information of the data transfer
6. Test it numerous times and find out its memory usage.

To install and run:
1. Download ProjectB.txt
2. Open Terminal
3. Run ProjectB.txt with terminal
4. Alternatively, copy command in text file and run it in the terminal

