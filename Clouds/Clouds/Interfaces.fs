module Interfaces

type DaylightType =
    | Morning
    | Noon
    | Evening
    | Night

type CreatureType =
    | Puppy
    | Kitten
    | Hedgehog
    | Bearcub
    | Piglet
    | Bat
    | Balloon

type IDaylight =
    abstract member Current: DaylightType with get

type ILuminary =
    abstract member IsShining: bool with get

type IWind =
    abstract member Speed: int with get

type ICreature =
    abstract member Type: CreatureType with get

type ICourier =
    abstract member GiveBaby: ICreature -> unit

type IMagic =
    abstract member CallStork: unit -> ICourier
    abstract member CallDaemon: unit -> ICourier