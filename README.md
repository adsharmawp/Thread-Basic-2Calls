# Thread-Basic-2Calls
Call two long time consuming methods on various kind of threads options

We have various options in C# threading to call long time consuming methods.
In this example you can see 4 diffrent ways to call these kind of methods.

We have 2 long time taking methods and get the result in integer; we need to get the result from both add the result and show it on UI asyncronously.

We implemented this requirement in following way:

* Using classing Thread Class
* Using TPL Task Class
* Using BackgroundWorker
* Using Async, Await option
