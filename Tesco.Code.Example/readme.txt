
Decorates implementations of the IAuthorisationService
with logging of average execution time and number of executions.
Use of generics is a little nasty, but it reduces type name length somewhat.

One problem with this implementation is that I am attempting to control access to 
shared static variables manually, which is error-prone. Another problem is 
that when the application shuts down then upto MaximumSampleSize timings may 
never have a corresponding average logged.

Depending on the nature of the application within which the code sits, a different 
"buffer" location for the timing information might be preferred: for example 
an external tuple store.

Finally, Windows exposes performance counters to the .NET framework which might 
be of use here.

//outside of the lock to minimise the length of the critical section
            //logging performance is of course very important (if it blocks)!
            //a non-blocking queue might be used here
