//sumfib

let sumFib = 
    let rec sumFib' sum (x, y) con num =
        if x + sum > num then
            sum 
        else 
            if con x then
                sumFib' (x + sum) (y, x+y) con num 
            else 
                sumFib' sum (y, x+y) con num
    sumFib' 0 (1, 1)

printfn "%A\n" (sumFib (fun x -> x % 2 = 0) 4000000)

//div

let prim =
    let rec prim' res num =
        if (res = num) then 
            res
        else 
            if (num % res = 0L) then 
                prim' res (num / res)
            else 
                prim' (res + 1L) num
    prim' 2L
 
printfn "%A\n" (prim 600851475143L)

//Fact

let fact =
    let rec fact' res num =
        if num > 1I then fact' (res*num) (num - 1I) else res
    fact' 1I

let sumDigits =
    let rec sumDigits' res num =
        match num with
        | _ when num = 0I -> res
        | _ -> sumDigits' (res + (num % 10I)) (num / 10I)
    sumDigits' 0I

let sumFact num = sumDigits (fact num)

printfn "%A\n" (sumFact 100I)

//Количество путей от (0, 0) до (20, 20)

(*
    Можно составить треугольник паскаля и взять число на позиции (20, 20),
    но воспользовавшись знаниями о биноме ньютона, можно сказать сразу, что
    там стоит сочетание из (20 - 1) по (20*2 - 2), т.е. сочетание из 19 по 38.
*)

printf "%A" (fact(38I)/(fact(19I)*fact(19I)))

//Arithm

type Expr = 
    | Const of int
    | Var of string
    | Add of Expr * Expr
    | Sub of Expr * Expr
    | Mul of Expr * Expr
    | Div of Expr * Expr

let rec recdesc expr =
    let add' el er =
        let el' = recdesc el
        let er' = recdesc er
        match el', er' with
        | Const 0, Var x | Var x, Const 0  -> Var "x"
        | Const x, Const y -> Const (x + y)
        | _ -> Add (el', er')
    let sub' el er =
        let el' = recdesc el
        let er' = recdesc er
        match el', er' with
        | Var x, Const 0  -> Var "x"
        | Const x, Const y -> Const (x - y)
        | Var x, Var y when x = y -> Const 0
        | _ -> Sub(el', er')
    let mul' el er =
        let el' = recdesc el
        let er' = recdesc er
        match el', er' with
        | Const 0, _ | _ , Const 0  -> Const 0
        | Const 1, x | x , Const 1  -> x
        | Const x, Const y -> Const (x * y)
        | _ -> Mul (el', er')
    let div' el er =
        let el' = recdesc el
        let er' = recdesc er
        match el', er' with
        | x , Const 1  -> x
        | Const x, Const y -> Const (x / y)
        | _ -> Div (el', er')  
    match expr with
    |Add(x, y) -> add' x y 
    |Sub(x, y) -> sub' x y
    |Mul(x, y) -> mul' x y
    |Div(x, y) -> div' x y
    |x -> x

let expr = (Mul (Add (Var "x", Const 0), Const 1)) in
    printf "%A" (recdesc expr)