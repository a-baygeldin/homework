module Example
    
    open MyMap
    open System
    open System.Diagnostics

    let mutable myDictionary = MyMap.Map<_, _> [|(2, "b"); (3, "c"); (4, "d")|]
    myDictionary <- myDictionary.Add 1 "a"
    myDictionary <- myDictionary.Remove 4
    printfn "%A" (myDictionary.Item(1))
    for i in myDictionary do printfn "%A" i

    //Speed test

    let mutable myMap = new MyMap.Map<int, string>([])
    let mutable systemMap = new Map<int, string>([])
     
    let stopWatch = Stopwatch.StartNew()
    for i in [1 .. 1000] do
        myMap <- myMap.Add i ("value" + i.ToString())
    stopWatch.Stop()
     
    printfn "%A" stopWatch.Elapsed.TotalMilliseconds
     
    let stopWatchSystem = Stopwatch.StartNew()
    for i in [1 .. 1000] do
        systemMap <- systemMap.Add i ("value" + i.ToString())
    stopWatchSystem.Stop()
     
    printfn "%A" stopWatchSystem.Elapsed.TotalMilliseconds

    Console.ReadLine() |> ignore

