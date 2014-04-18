(*
    A. Baygeldin (c)  2014
    OOP
*)

module Pokemon

[<AbstractClass>]
type Named (name:string) =
    member this.Name = name
    abstract member SetName : string -> unit
    override this.ToString() = "Unknown " + name

type Master (name:string) = 
    inherit Named(name)
    let mutable name = name
    let mutable sausages = 0;
    let mutable flappy = 0;

    override this.SetName newname = 
        printfn "Probably %A has quit the game and his pokemons goes to %A hands" name newname
        name <- newname
    
    member this.Dice =
        let rnd = new System.Random(int System.DateTime.Now.Ticks)
        let throw = rnd.Next(1, 6)
        printfn "%A throws %A" name throw
        throw

    member this.FrySausages = 
        sausages <- sausages + 1
        printfn "%A is frying sausages. Tasty!" name

    member this.PlayFlappyBird = 
        flappy <- flappy + 1
        printfn "%A is playing Flappy Bird. Lose again! D-oh!" name

[<AbstractClass>]
type Pokemon (name:string, master:Master, ability:string, damage:int) = 
    inherit Named(name)
    let mutable name = name
    let mutable master = master
    let mutable level = 1
    let mutable hp = 100

    abstract member Special : unit
    override this.SetName newname = 
        printfn "Probably %A has become strong enough to call himself %A" name newname
        name <- newname
    override this.ToString() = "Unknown Pokemon" + name

    member this.HP = hp
    member this.Master = master
    member this.Hurt damage = hp <- (hp - damage)
    member this.LevelUp = level <- level + 1
    member this.ChangeMaster (newmaster:Master) = master <- newmaster
    member this.Attack (pokemon:Pokemon) =
        printfn "%A attacks %A with %A and cause damage %A" name pokemon.Name ability damage
        pokemon.Hurt damage
        if pokemon.HP <= 0 then 
            this.LevelUp
            pokemon.ChangeMaster this.Master

type FirePokemon (name:string, master:Master, ability:string, damage:int) = 
    inherit Pokemon(name, master, ability, damage)

    override this.ToString() = "Fire Pokemon" + name
    override this.Special = 
        printfn "%A is blazing with fire. ROWR!" name
        master.FrySausages

type ElectricPokemon (name:string, master:Master, ability:string, damage:int) = 
    inherit Pokemon(name, master, ability, damage)

    override this.ToString() = "Electric Pokemon" + name
    override this.Special = 
        printfn "%A is charging his master's cellphone. Still 99%% and it will never be 100%%..." name
        master.PlayFlappyBird


let firstMaster = new Master("Ash")
let secondMaster = new Master("Misty")
let firstPoke = new ElectricPokemon ("Pikachu", firstMaster, "Lightningrod", 10)
let secondPoke = new FirePokemon ("Charmander", secondMaster, "Solar Power", 10)

firstPoke.Special
secondPoke.Special

let rec Battle (first:Pokemon) (second:Pokemon) =
    if first.HP <= 0 then printfn "%A win!" second.Name
    if second.HP <= 0 then printfn "%A win!" first.Name
    if first.Master <> second.Master then
        let firstThrow = first.Master.Dice
        let secondThrow = second.Master.Dice
        if firstThrow > secondThrow then
            first.Attack second
        else if firstThrow < secondThrow then
            second.Attack first
        Battle first second

Battle firstPoke secondPoke