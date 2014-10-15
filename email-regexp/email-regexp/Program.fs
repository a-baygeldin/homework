(*
    A. Baygeldin (c) 2014
    NUnit tests
*)

namespace emailRegexp

open NUnit.Framework
open System.Text.RegularExpressions

[<TestFixture>]   
module tests =
    //regex101.com - awesome!
    let regexp = "^[a-zA-Z]+([\.]?[\w]+)+@([a-zA-Z]+([\-]?[a-zA-Z]+)+\.)+(aero|arpa|asia|biz|cat|com|ru|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|post|pro|tel|travel|xxx)$"
    let checkEmail str = Regex.IsMatch(str, regexp)
    
    [<Test>]
    let test1() = Assert.IsTrue (checkEmail "l.o.l@l-o-l.ru")

    [<Test>]
    let test2() = Assert.IsTrue (checkEmail "myemail@google.asia")

    [<Test>]
    let test3() = Assert.IsTrue (checkEmail "myemail@google.moogle.shmoogle.asia")

    [<Test>]
    let test4() = Assert.IsFalse (checkEmail "l..o.l@l-o-l.ru")

    [<Test>]
    let test5() = Assert.IsFalse (checkEmail "l.o.l@l--o-l.ru")

    [<Test>]
    let test6() = Assert.IsFalse (checkEmail "l.o.l@l-o-l.ururu")

    [<Test>]
    let test7() = Assert.IsFalse (checkEmail "lol.@l-o-l.ru")

    [<Test>]
    let test8() = Assert.IsFalse (checkEmail "lol@lol-.ru")

    [<Test>]
    let test9() = Assert.IsFalse (checkEmail "0lol@lol.ru")

    [<Test>]
    let test10() = Assert.IsFalse (checkEmail "0lol@lol.ru")

    [<Test>]
    let test11() = Assert.IsFalse (checkEmail "lol@l0l.ru")

    [<Test>]
    let test12() = Assert.IsFalse (checkEmail "o@lol.ru")

    [<Test>]
    let test13() = Assert.IsFalse (checkEmail "lol@o.ru")

    [<Test>]
    let test14() = Assert.IsFalse (checkEmail "customer/department=shipping@example.com")

    [<Test>]
    let test15() = Assert.IsFalse (checkEmail "abc@def@example.com")

    [<Test>]
    let test16() = Assert.IsTrue (checkEmail "abc@example.asia")

    [<Test>]
    let test17() = Assert.IsTrue (checkEmail "abc@example.museum")