public class Worker : BackgroundService
{
    private bool processMessagePump = true;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Create a new thread for the message pump
        Thread t = new Thread(new ThreadStart(messagePump));
        t.Start();

        // Wait for the worker thread to exit
        await t.JoinAsync();

        // Close the queue and disconnect from the queue manager
        queue.Close();
        qMgr.Disconnect();
    }

    private void messagePump()
    {
        // Create a connection factory object
        MQConnectionFactory cf = new MQConnectionFactory();

        // Set the properties of the connection factory object
        cf.HostName = "localhost";
        cf.Port = 1414;
        cf.Channel = "SYSTEM.DEF.SVRCONN";
        cf.QueueManager = "QMGR";

        // Create a connection object
        MQConnection conn = (MQConnection)cf.CreateConnection();

        // Create a session object
        MQSession sess = (MQSession)conn.CreateSession(false, AcknowledgeMode.AutoAcknowledge);

        // Create a destination object
        MQDestination dest = (MQDestination)sess.CreateQueue("QUEUE");

        // Create a consumer object
        MQConsumer consumer = (MQConsumer)sess.CreateConsumer(dest);

        // Loop forever, blocking on the call to Receive or until a timeout occurs
        while (processMessagePump)
        {
            try
            {
                // Receive the next message from the queue
                MQMessage msg = (MQMessage)consumer.Receive(5000);

                // Process the message here

                // Reset the message object for reuse
                msg.ClearMessage();
            }
            catch (MQException mqe)
            {
                if (mqe.ReasonCode == MQC.MQRC_NO_MSG_AVAILABLE)
                {
                    // No messages available - continue looping
                    continue;
                }
                else
                {
                    // Some other error occurred - stop looping
                    processMessagePump = false;
                    break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // Close the consumer, session and connection objects
        consumer.Close();
        sess.Close();
        conn.Close();
    }
}
