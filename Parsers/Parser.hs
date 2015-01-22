module Parser where

import ParserCombinators
import qualified Tokenizer

--X - variable, C- constant, A - add, S - subtraction, M - multiplicate, D - divide
data T = X String | C Int | A T T | S T T | M T T | D T T deriving Show

instance Eq Tokenizer.Token where
  x == y = (ord x) == (ord y) where
    ord (Tokenizer.X _) = 0
    ord (Tokenizer.C _) = 1
    ord  Tokenizer.A    = 2
    ord  Tokenizer.S    = 3
    ord  Tokenizer.M    = 4
    ord  Tokenizer.D    = 5
    ord  Tokenizer.LP   = 6
    ord  Tokenizer.RP   = 7

--Парсит + или - и заменяет их на рабочие + и - (а не просто токены)
addi = sym Tokenizer.A ||> lift (val A) |||
       sym Tokenizer.S ||> lift (val S)

--То же, но для * и /
multi = sym Tokenizer.M ||> lift (val M) |||
        sym Tokenizer.D ||> lift (val D)

--Т.к. мы переопределили равенство, то все рабает норм (небольшой костыль)
--Делает то же, но для переменной и константы
var = sym (Tokenizer.X "") ||> (\ (Tokenizer.X s) -> val (X s))
int = sym (Tokenizer.C 0 ) ||> (\ (Tokenizer.C i) -> val (C i)) 
lp  = sym Tokenizer.LP
rp  = sym Tokenizer.RP

{-
a = b + 5
b = 4
Это нормально, т.к. хаскель ленивый.
-}

--разбирает многочлен
e = m ||| m ||> (\ x -> addi  ||> (\ op -> e ||> (\y -> val $ x `op` y )))
--разбирает одночлен
m = p ||| p ||> (\ x -> multi ||> (\ op -> m ||> (\y -> val $ x `op` y )))

--разбирает выражение (переменная, константа или выражение в скобках)
p = var ||| 
    int ||| 
    lp ||> lift (e ||> (\e -> rp ||> lift (val e)))

--да, парсер не умеет в числа больше одной цифры
parse = apply e . Tokenizer.tokenize
 