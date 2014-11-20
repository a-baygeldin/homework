module Program

open Shop

[<EntryPoint>]

let main argv =
    let shop = new Shop.AlkoShop()
    let customer = new Customer.Customer(new Customer.Calendar())

    customer.GoShopping shop
    customer.GetDrunk ()
    printfn "%A" <| customer.IsDrunk

    0