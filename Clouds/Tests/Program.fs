module Tests 

open NUnit.Framework
open NSubstitute
open FsCheck
open FsUnit
open Cloud
open Interfaces

type ``int from 0 to 2`` =
        static member int() = Arb.fromGen <| Gen.choose(0, 2)

type ``int from 4 to 6`` =
        static member int() = Arb.fromGen <| Gen.choose(4, 6)

type ``int from 6 to 8`` =
        static member int() = Arb.fromGen <| Gen.choose(6, 8)

type ``int from 8 to 10`` =
        static member int() = Arb.fromGen <| Gen.choose(8, 10)

type ``int from 0 to 10`` =
        static member int() = Arb.fromGen <| Gen.choose(0, 10)

let mockAll (daylight : DaylightType) (luminary : bool) (wind : int) = 
    
    let mockMagic = Substitute.For<IMagic>()
    let mockCourier = Substitute.For<ICourier>()
    let mockDayLight = Substitute.For<IDaylight>()
    let mockLuminary = Substitute.For<ILuminary>()
    let mockWind = Substitute.For<IWind>()
    let cloud = new Cloud.Cloud(mockDayLight, mockLuminary, mockWind, mockMagic)

    mockDayLight.Current.Returns(daylight) |> ignore
    mockLuminary.IsShining.Returns(luminary) |> ignore
    mockWind.Speed.Returns(wind) |> ignore
    mockMagic.CallStork().Returns(mockCourier) |> ignore
    mockMagic.CallDaemon().Returns(mockCourier) |> ignore

    (cloud, mockMagic, mockCourier)


[<Test>]
let ``Call Stork and return Puppy``() =
    let test wind = 
        let cloud, magic, courier = mockAll DaylightType.Morning true wind
    
        cloud.Create() |> ignore

        Received.InOrder(fun () ->
            magic.CallStork() |> ignore
            courier.GiveBaby(Arg.Is<ICreature>(fun (creature : ICreature) -> creature.Type = Puppy)) |> ignore
        )

    Arb.register<``int from 0 to 2``>() |> ignore
    Check.Quick test

[<Test>]
let ``Call Stork and return Kitten``() =
    let test wind = 
        let cloud, magic, courier = mockAll DaylightType.Evening true wind
    
        cloud.Create() |> ignore

        Received.InOrder(fun () ->
            magic.CallStork() |> ignore
            courier.GiveBaby(Arg.Is<ICreature>(fun (creature : ICreature) -> creature.Type = Kitten)) |> ignore
        )

    Arb.register<``int from 0 to 2``>() |> ignore
    Check.Quick test

[<Test>]
let ``Call Daemon and return Hedgehog``() =
    let test wind = 
        let cloud, magic, courier = mockAll DaylightType.Night false wind
    
        cloud.Create() |> ignore

        Received.InOrder(fun () ->
            magic.CallDaemon() |> ignore
            courier.GiveBaby(Arg.Is<ICreature>(fun (creature : ICreature) -> creature.Type = Hedgehog)) |> ignore
        )

    Arb.register<``int from 8 to 10``>() |> ignore
    Check.Quick test

[<Test>]
let ``Call Daemon and return Beearcub``() =
    let test wind = 
        let cloud, magic, courier = mockAll DaylightType.Noon true wind
    
        cloud.Create() |> ignore

        Received.InOrder(fun () ->
            magic.CallDaemon() |> ignore
            courier.GiveBaby(Arg.Is<ICreature>(fun (creature : ICreature) -> creature.Type = Bearcub)) |> ignore
        )

    Arb.register<``int from 6 to 8``>() |> ignore
    Check.Quick test

[<Test>]
let ``Call Stork and return Piglet``() =
    let test wind = 
        let cloud, magic, courier = mockAll DaylightType.Noon true wind
    
        cloud.Create() |> ignore

        Received.InOrder(fun () ->
            magic.CallStork() |> ignore
            courier.GiveBaby(Arg.Is<ICreature>(fun (creature : ICreature) -> creature.Type = Piglet)) |> ignore
        )

    Arb.register<``int from 4 to 6``>() |> ignore
    Check.Quick test

[<Test>]
let ``Call Daemon and return Bat``() =
    let test wind = 
        let cloud, magic, courier = mockAll DaylightType.Night false wind
    
        cloud.Create() |> ignore

        Received.InOrder(fun () ->
            magic.CallDaemon() |> ignore
            courier.GiveBaby(Arg.Is<ICreature>(fun (creature : ICreature) -> creature.Type = Bat)) |> ignore
        )

    Arb.register<``int from 0 to 2``>() |> ignore
    Check.Quick test

[<Test>]
let ``Call Stork and return Balloon`` () =
    let test wind = 
        let cloud, magic, courier = mockAll DaylightType.Night false wind
    
        cloud.Create() |> ignore

        Received.InOrder(fun () ->
            magic.CallStork() |> ignore
            courier.GiveBaby(Arg.Is<ICreature>(fun (creature : ICreature) -> creature.Type = Balloon)) |> ignore
        )

    Arb.register<``int from 0 to 10``>() |> ignore
    Check.Quick test