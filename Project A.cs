using System.Diagnostics;

class BankAccount(int id, int initialBalance) { //class for storing methods and info of the back account
    public int Id { get; } = id; //obtains the id of the bank account
    private int balance = initialBalance; //stores the current balance of the account
    private readonly Mutex mutex = new(); //mutex object for synchronization and ensuring only one thread is modifying the bank account
    private readonly Stopwatch stopwatch = new(); //initializes stopwatch object for tracking time elapsed

    public void Deposit(int amount) { //method for depositing money into the account
        stopwatch.Start(); //starts the stopwatch to track thread elapsed time
        mutex.WaitOne(); //acquires the lock for the thread to ensure it remains safe
        try {
            balance += amount; //increases the balance by the amount given in the main method
            Console.WriteLine($"Thread [{Environment.CurrentManagedThreadId}] deposited {amount:C} into Account {Id}. New balance: {balance:C}.");
        } finally {
            mutex.ReleaseMutex(); //releases the lock for the thread so other threads can continue
            stopwatch.Stop(); //stops measuring the time it takes for the thread to complete
            Console.WriteLine($"Thread [{Environment.CurrentManagedThreadId}] duration: {stopwatch.ElapsedMilliseconds} ms.");
        }
    }

    public void Withdraw(int amount) { //method for withdrawing money from the account
        stopwatch.Start(); //starts the stopwatch to track thread elapsed time
        mutex.WaitOne(); //acquires the lock for the thread to ensure it remains safe
        try {
            if (balance >= amount) { //runs if current balance is higher than the amount given
                balance -= amount; //lowers the balance by the amount given in the main method
                Console.WriteLine($"Thread [{Environment.CurrentManagedThreadId}] withdrew {amount:C} from Account {Id}. New balance: {balance:C}.");
            } else { //error handling if the amount given is higher than the current balance
                Console.WriteLine($"Thread [{Environment.CurrentManagedThreadId}] withdrawal of {amount:C} from Account {Id} failed due to insufficient funds.");
            }
        } finally {
            mutex.ReleaseMutex(); //releases the lock for the thread so other threads can continue
            stopwatch.Stop(); //stops measuring the time it takes for the thread to complete
            Console.WriteLine($"Thread [{Environment.CurrentManagedThreadId}] duration: {stopwatch.ElapsedMilliseconds} ms.");
        }
    }

    public void Transfer(BankAccount target, int amount) { //method for transferring funds from one account to another
        stopwatch.Start(); //starts the stopwatch to track the threads' elapsed time

        BankAccount first = this.Id < target.Id ? this : target; //determines the target bank account
        BankAccount second = this.Id < target.Id ? target : this; //determines the target bank account

        bool acquiredFirstLock = false; //initializes field to track lock status of first account
        bool acquiredSecondLock = false; //initializes field to track lock status of second account

        try {
            acquiredFirstLock = first.mutex.WaitOne(TimeSpan.FromMilliseconds(100)); //acquires the lock for the first account 
            acquiredSecondLock = second.mutex.WaitOne(TimeSpan.FromMilliseconds(100)); //acquires the lock for the second account

            if (acquiredFirstLock && acquiredSecondLock) { //condition that is met if both accounts are locked
                if (balance >= amount) { //runs if current balance is higher than the amount transferred
                    balance -= amount; //lowers the balance by the amount given in the main method
                    target.balance += amount; //adds the amount to the target bank account
                    Console.WriteLine($"Thread [{Environment.CurrentManagedThreadId}] transferred {amount:C} from Account {Id} to Account {target.Id}.");
                } else { //error handling if the amount given is higher than the current balance
                    Console.WriteLine($"Thread [{Environment.CurrentManagedThreadId}] transfer of {amount:C} from Account {Id} to Account {target.Id} failed due to insufficient funds.");
                }
            } else { //error handling if the locks were not acquired within the specified 100 ms
                Console.WriteLine($"Thread [{Environment.CurrentManagedThreadId}] Deadlock prevented: Transfer from Account {Id} to Account {target.Id}.");
            }
        } finally {
            if (acquiredFirstLock) first.mutex.ReleaseMutex(); //releases the first lock if it was acquired
            if (acquiredSecondLock) second.mutex.ReleaseMutex(); //releases the second lock if it was acquired
            stopwatch.Stop(); //stops measuring the time it takes for the threads to complete
            Console.WriteLine($"Thread [{Environment.CurrentManagedThreadId}] duration: {stopwatch.ElapsedMilliseconds} ms.");
        }
    }
}

public class Program { //main class for testing
    public static void Main() { //main method to run program
        BankAccount account1 = new(1, 1000); //initializes first bank account with a initial balance of $1000
        BankAccount account2 = new(2, 1000); //initializes second bank account with a initial balance of $1000

        Thread[] threads = //array of 10 different threads/actions that each affect the bank accounts
        [
            new Thread(() => account1.Transfer(account2, 300)), //transfers $300 from account 1 to account 2
            new Thread(() => account2.Withdraw(500)), //withdraws $500 from account 2
            new Thread(() => account1.Transfer(account2, 200)), //transfers $200 from account 1 to account 2
            new Thread(() => account2.Deposit(400)), //deposits $400 into account 2
            new Thread(() => account1.Deposit(150)), //deposits $150 into account 1
            new Thread(() => account2.Transfer(account1, 250)), //transfers $250 from account 2 to account 1
            new Thread(() => account1.Transfer(account2, 350)), //transfers $350 from account 1 to account 2 
            new Thread(() => account2.Transfer(account1, 100)), //transfers $100 from account 2 to account 1
            new Thread(() => account1.Withdraw(450)), //withdraws $450 from account 1
            new Thread(() => account2.Transfer(account1, 550)), //transfers $550 from account 2 to account 1
        ];

        foreach (Thread t in threads) { //starts all 10 threads                         
            t.Start();
        }

        foreach (Thread t in threads) {//waits for the threads to finish so the program can finish
            t.Join();
        }

        Console.WriteLine("All transactions completed."); 
    }
}
