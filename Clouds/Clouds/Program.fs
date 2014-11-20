module Cloud

open Interfaces

type Daylight() =
    interface IDaylight with
        member x.Current = Morning

type Luminary() = 
    interface ILuminary with
        member x.IsShining = true

type Wind() =
    interface IWind with
        member x.Speed = 2

type Daemon() =
    interface ICourier with
        member x.GiveBaby creature = ignore()

type Stork() =
    interface ICourier with
        member x.GiveBaby creature = ignore()
     
type Magic() =
    interface IMagic with
        member x.CallDaemon() = new Daemon() :> ICourier
        member x.CallStork() = new Stork() :> ICourier

type Creature(creature) = 
    interface ICreature with
        member x.Type = creature


type Cloud(daylight : IDaylight, luminary: ILuminary, wind : IWind, magic: IMagic) =
    
    member private x.InternalCreate() =
        match daylight.Current, luminary.IsShining, wind.Speed with
        | DaylightType.Morning, true, x when (x >= 0) && (x <= 2) ->
            new Creature(CreatureType.Puppy)

        | DaylightType.Evening, true, x when (x >= 0) && (x <= 2) ->
            new Creature(CreatureType.Kitten)

        | DaylightType.Night, false, x when (x >= 8) && (x <= 10) ->
            new Creature(CreatureType.Hedgehog)

        | DaylightType.Noon, true, x when (x >= 6) && (x <= 8) ->
            new Creature(CreatureType.Bearcub)

        | DaylightType.Noon, true, x when (x >= 4) && (x <= 6) ->
            new Creature(CreatureType.Piglet)

        | DaylightType.Night, false, x when (x >= 0) && (x <= 2) ->
            new Creature(CreatureType.Bat)

        | _ ->
            new Creature(CreatureType.Balloon)
   
    member x.Create() =
        let creature = x.InternalCreate() :> ICreature
        
        match creature.Type with
        | Puppy | Kitten | Piglet | Balloon -> magic.CallStork().GiveBaby(creature)
        | Hedgehog | Bearcub | Bat -> magic.CallDaemon().GiveBaby(creature)

        creature