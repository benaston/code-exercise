For this exercise I decided to see how far I could get with the decorator pattern in the time I had available.

I test-drove the implementation of the execution time decorator. I'll hold my hands up and admit that, due to time constraints I then implemented the ExecutionTimeDecorator directly.

One problem with this implementation is that I am attempting to control access to shared state manually, which is error-prone. Another problem is 
that when the application shuts down, the in memory buffers will not be flushed.

Finally, Windows exposes performance counters to the .NET framework which might be of use here.