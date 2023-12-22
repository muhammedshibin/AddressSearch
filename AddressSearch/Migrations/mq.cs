class SampleMessageListener : IMessageListener
{
    public void OnMessage(IMessage message)
    {
        try
        {
            ITextMessage textMessage = (ITextMessage)message;

            // Process the message
            ProcessMessage(textMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing message: {ex.Message}");

            // Implement your retry logic here
            int maxRetries = 3;
            int currentRetry = 0;

            while (currentRetry < maxRetries)
            {
                try
                {
                    // Retry processing the message
                    ProcessMessage((ITextMessage)message);
                    break;
                }
                catch (Exception retryEx)
                {
                    Console.WriteLine($"Retry {currentRetry + 1}/{maxRetries} failed: {retryEx.Message}");
                    currentRetry++;
                    Thread.Sleep(1000); // Add a delay before retrying
                }
            }
        }
    }

    static void ProcessMessage(ITextMessage message)
    {
        // Implement your message processing logic here
        string text = message.Text;
        Console.WriteLine($"Received message: {text}");
    }
}
