using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Enumeration;

namespace TAFL
{
    public class Interpreter
    {
        private Stack<CallStackElem> _callStack = new Stack<CallStackElem>();
        private Scope CurrentScope => _callStack.Peek().Scope;
        private int _k;
        private Stack<Elem> _magaszine = new Stack<Elem>();
        private List<OPSElem> _ops;
        private Interpreter(List<OPSElem> ops)
        {
            _enumerator = _doubles().GetEnumerator();
            _ops = ops;
            _callStack.Push(new CallStackElem(null, -1));
        }

        private IEnumerator<double> _enumerator;
        private IEnumerable<double> _doubles()
        {
            while (true)
            {
                var line = Console.ReadLine();
                foreach (var str in line.Split(' '))
                    yield return Double.Parse(str, CultureInfo.InvariantCulture);
            }
        }

        private double _ReadDouble()
        {
            _enumerator.MoveNext();
            return _enumerator.Current;
        }
        class CallStackElem
        {
            public CallStackElem(Scope previous, int returnTo)
            {
                Scope = new Scope(previous);
                ReturnTo = returnTo;
            }
            public Scope Scope;
            public int ReturnTo;
        }

        class Scope
        {
            private Dictionary<string, Elem> _variebles = new Dictionary<string, Elem>();
            private Dictionary<string, Function> _functions = new Dictionary<string, Function>();
            public Scope Previous;
            public Scope(Scope previous) => Previous = previous;
            public Elem? FindVariable(string name)
            {
                var scope = this;
                while (scope != null)
                {
                    if (scope._variebles.ContainsKey(name)) return scope._variebles[name];
                    scope = scope.Previous;
                }

                return null;
            }

            public void SetVariable(string name, Elem value)
            {
                var scope = this;
                while (scope != null)
                {
                    if (scope._variebles.ContainsKey(name))
                    {
                        scope._variebles[name] = value;
                        return;
                    }
                    scope = scope.Previous;
                }

                throw new Exception($"Variable {name} not found");
            }

            public void AddVariable(string name, Elem value) => _variebles.Add(name, value);

            public void AddFunction(string name, Function function) => _functions.Add(name, function);
            
            public Function? FindFunction(string name)
            {
                var scope = this;
                while (scope != null)
                {
                    if (scope._functions.ContainsKey(name)) return scope._functions[name];
                    scope = scope.Previous;
                }

                return null;
            }
        }
        struct Elem
        {
            public ElemType Type;
            public double X;
            public string Name;
            private Elem[] _elems;
            private Scope _scope;
            public int Length => _elems.Length;

            public static Elem MakeValue(double x) => new Elem() {
                Type = ElemType.Value,
                X = x,
            };

            public static Elem MakeIdentifier(string name, Scope scope) => new Elem() {
                Type = ElemType.Identifier,
                Name = name,
                _scope = scope
            };

            public static Elem MakeArray(int n)
            {
                Elem elem = new Elem()
                {
                    Type = ElemType.Array,
                    _elems = new Elem[n]
                };
                for (int i = 0; i < n; i++)
                {
                    elem._elems[i] = MakeValue(0);
                }
                return elem;
            }

            public static Elem MakeArray(Elem elem)
            {
                if (elem.Type != ElemType.Value) throw new Exception("N is not a number");
                return MakeArray((int) elem.X);
            }
            public Elem this[Elem i]
            {
                get
                {
                    if (Type != ElemType.Array) throw new Exception("Trying index not array");
                    if (i.Type == ElemType.Array) throw new Exception("Trying index array with array");
                    return new Elem()
                    {
                        Type = ElemType.Indexed,
                        _elems = this._elems,
                        X = (int) i.X,
                    };
                }
            }

            public static Elem MakeArg() => new Elem() {Type = ElemType.Arg};
            
            public enum ElemType
            {
                Identifier,
                Value,
                Array,
                Indexed,
                Arg
            }
            
            public Elem Get()
            {
                switch (Type)
                {
                    case ElemType.Value:
                        return this;
                    case ElemType.Array:
                        return this;
                    case ElemType.Indexed:
                        return _elems[(int)X];
                    case ElemType.Identifier:
                        var elem = _scope.FindVariable(Name);
                        if (elem == null) throw new Exception($"Variable {Name} not found");
                        else return elem.Value;
                    default:
                        throw new Exception("Something goes wrong");
                }
            }

            public void Set(Elem elem)
            {
                switch (Type)
                {
                    case ElemType.Identifier:
                        _scope.SetVariable(Name, elem);
                        break;
                    case ElemType.Indexed:
                        _elems[(int) X] = elem;
                        break;
                    default:
                        throw new Exception("Something goes wrong");
                }
            }

            public void AddVariable(Elem elem)
            {
                if (Type != ElemType.Identifier) throw new Exception("Something goes wrong");
                _scope.AddVariable(Name, elem);
            }

            public void AddFunction(Function fun)
            {
                if (Type != ElemType.Identifier) throw new Exception("Something goes wrong");
                _scope.AddFunction(Name,fun);
            }

            public Function GetFunction()
            {
                if (Type != ElemType.Identifier) throw new Exception("Something goes wrong");
                var fun = _scope.FindFunction(Name);
                if (fun == null) throw new Exception($"Function {Name} not found");
                return fun.Value;
            }
            #region Операторы
            public static Elem operator+ (Elem l, Elem r)
            {
                Elem res;
                switch ((l.Type, r.Type))
                {
                    case (ElemType.Value, ElemType.Value):
                        l.X += r.X;
                        return l;
                    case (ElemType.Array, ElemType.Array):
                        var min = Math.Min(l.Length, r.Length);
                        var max = Math.Max(l.Length, r.Length);
                        res = MakeArray(max);
                        for (int i = 0; i < min; i++)
                            res._elems[i] = l._elems[i] + r._elems[i];
                        for (int i = min; i < max; i++)
                            res._elems[i] = (l.Length > r.Length) switch
                            {
                                true => l._elems[i],
                                false => r._elems[i]
                            };
                        return res;
                    case (ElemType.Value, ElemType.Array):
                        res = Elem.MakeArray(r.Length);
                        for (int i = 0; i < r.Length; i++)
                            res._elems[i] = l + r._elems[i];
                        return res;
                    case (ElemType.Array, ElemType.Value):
                        res = Elem.MakeArray(l.Length);
                        for (int i = 0; i < l.Length; i++)
                            res._elems[i] = l._elems[i] + r;
                        return res;
                    default:
                        throw new Exception("Something goes wrong");
                }
            }
            public static Elem operator- (Elem l, Elem r)
            {
                Elem res;
                switch ((l.Type, r.Type))
                {
                    case (ElemType.Value, ElemType.Value):
                        l.X -= r.X;
                        return l;
                    case (ElemType.Array, ElemType.Array):
                        var min = Math.Min(l.Length, r.Length);
                        var max = Math.Max(l.Length, r.Length);
                        res = MakeArray(max);
                        for (int i = 0; i < min; i++)
                            res._elems[i] = l._elems[i] - r._elems[i];
                        for (int i = min; i < max; i++)
                            res._elems[i] = (l.Length > r.Length) switch
                            {
                                true => l._elems[i],
                                false => Elem.MakeValue(0) - r._elems[i]
                            };
                        return res;
                    case (ElemType.Value, ElemType.Array):
                        res = Elem.MakeArray(r.Length);
                        for (int i = 0; i < r.Length; i++)
                            res._elems[i] = l - r._elems[i];
                        return res;
                    case (ElemType.Array, ElemType.Value):
                        res = Elem.MakeArray(l.Length);
                        for (int i = 0; i < l.Length; i++)
                            res._elems[i] = l._elems[i] - r;
                        return res;
                    default:
                        throw new Exception("Something goes wrong");
                }
            }
            public static Elem operator* (Elem l, Elem r)
            {
                Elem res;
                switch ((l.Type, r.Type))
                {
                    case (ElemType.Value, ElemType.Value):
                        l.X *= r.X;
                        return l;
                    case (ElemType.Array, ElemType.Array):
                        var min = Math.Min(l.Length, r.Length);
                        var max = Math.Max(l.Length, r.Length);
                        res = MakeArray(max);
                        for (int i = 0; i < min; i++)
                            res._elems[i] = l._elems[i] + r._elems[i];
                        for (int i = min; i < max; i++)
                            res._elems[i] = Elem.MakeValue(1) * (l.Length > r.Length) switch
                            {
                                true => l._elems[i],
                                false => r._elems[i]
                            };
                        return res;
                    case (ElemType.Value, ElemType.Array):
                        res = Elem.MakeArray(r.Length);
                        for (int i = 0; i < r.Length; i++)
                            res._elems[i] = l * r._elems[i];
                        return res;
                    case (ElemType.Array, ElemType.Value):
                        res = Elem.MakeArray(l.Length);
                        for (int i = 0; i < l.Length; i++)
                            res._elems[i] = l._elems[i] * r;
                        return res;
                    default:
                        throw new Exception("Something goes wrong");
                }
            }
            public static Elem operator/ (Elem l, Elem r)
            {
                Elem res;
                switch ((l.Type, r.Type))
                {
                    case (ElemType.Value, ElemType.Value):
                        l.X /= r.X;
                        return l;
                    case (ElemType.Array, ElemType.Array):
                        var min = Math.Min(l.Length, r.Length);
                        var max = Math.Max(l.Length, r.Length);
                        res = MakeArray(max);
                        for (int i = 0; i < min; i++)
                            res._elems[i] = l._elems[i] / r._elems[i];
                        for (int i = min; i < max; i++)
                            res._elems[i] = (l.Length > r.Length) switch
                            {
                                true => l._elems[i],
                                false => Elem.MakeValue(1) / r._elems[i]
                            };
                        return res;
                    case (ElemType.Value, ElemType.Array):
                        res = Elem.MakeArray(r.Length);
                        for (int i = 0; i < r.Length; i++)
                            res._elems[i] = l / r._elems[i];
                        return res;
                    case (ElemType.Array, ElemType.Value):
                        res = Elem.MakeArray(l.Length);
                        for (int i = 0; i < l.Length; i++)
                            res._elems[i] = l._elems[i] / r;
                        return res;
                    default:
                        throw new Exception("Something goes wrong");
                }
            }
            
            public static Elem operator- (Elem elem) => Elem.MakeValue(0) - elem;
            public static Elem operator! (Elem elem) => Elem.MakeValue(elem.Compare(Elem.MakeValue(0)) == 0 ? 1 : 0);
            
            
            public static Elem operator> (Elem l, Elem r) => Elem.MakeValue(l.Compare(r) > 0 ? 1 : 0);
            public static Elem operator< (Elem l, Elem r) => Elem.MakeValue(l.Compare(r) < 0 ? 1 : 0);
            public static Elem operator>= (Elem l, Elem r) => Elem.MakeValue(l.Compare(r) >= 0 ? 1 : 0);
            public static Elem operator<= (Elem l, Elem r) => Elem.MakeValue(l.Compare(r) <= 0 ? 1 : 0);
            public static Elem operator== (Elem l, Elem r) => Elem.MakeValue(l.Compare(r) == 0 ? 1 : 0);
            public static Elem operator!= (Elem l, Elem r) => Elem.MakeValue(l.Compare(r) != 0 ? 1 : 0);

            public static Elem operator& (Elem l, Elem r) => l.Compare(Elem.MakeValue(0)) switch {
                0 => l,
                _ => r
            };
            public static Elem operator| (Elem l, Elem r) => l.Compare(Elem.MakeValue(0)) switch {
                0 => r,
                _ => l
            };

            public int Compare(Elem r)
            {
                switch ((Type,r.Type))
                {
                    case (ElemType.Value, ElemType.Value):
                        return X.CompareTo(r.X);
                    case (ElemType.Array, ElemType.Array):
                    {
                        var min = Math.Min(Length, r.Length);
                        for (int i = 0; i < min; i++)
                        {
                            var c = _elems[i].Compare(r._elems[i]);
                            if (c != 0) return c;
                        }
                        return -1 * Length.CompareTo(r.Length);
                    }
                    case (ElemType.Value, ElemType.Array):
                        return X.CompareTo(r.Length);
                    case (ElemType.Array, ElemType.Value):
                        return r.X.CompareTo(X);
                    default:
                        throw new ArgumentException("Сравнение невозможно");
                }
            }
            #endregion
            public override string ToString() =>
                Type switch
                {
                    ElemType.Value => X.ToString(),
                    ElemType.Array => $"[{string.Join(", ", _elems)}]",
                    _ => throw new Exception("Something goes wrong")
                };
        }
        struct Function
        {
            public string[] Names;
            public int m;
        }
        
        public static void Interpret(List<OPSElem> ops) => new Interpreter(ops)._Interpret();
        private void _Interpret()
        {
            while (_k < _ops.Count)
            {
                _operations[_ops[_k].Type](this);
                _k++;
            }
        }

        private delegate void _Operation(Interpreter c);

        private static _Operation[] _operations =
        {
            (c) => { },
            (c) => c._magaszine.Push(Elem.MakeIdentifier(c._ops[c._k].Word, c._callStack.Peek().Scope)),
            (c) => c._magaszine.Push(Elem.MakeValue(c._ops[c._k].Value)),
            (c) =>
            {
                var right = c._magaszine.Pop().Get();
                var left = c._magaszine.Pop().Get();
                c._magaszine.Push(left + right);
            },
            (c) =>
            {
                var right = c._magaszine.Pop().Get();
                var left = c._magaszine.Pop().Get();
                c._magaszine.Push(left - right);
            },
            (c) =>
            {
                var right = c._magaszine.Pop().Get();
                c._magaszine.Push(-right);
            },
            (c) =>
            {
                var right = c._magaszine.Pop().Get();
                var left = c._magaszine.Pop().Get();
                c._magaszine.Push(left * right);
            },
            (c) =>
            {
                var right = c._magaszine.Pop().Get();
                var left = c._magaszine.Pop().Get();
                c._magaszine.Push(left / right);
            },
            (c) =>
            {
                var right = c._magaszine.Pop().Get();
                var left = c._magaszine.Pop();
                left.Set(right);
            },
            (c) =>
            {
                var right = c._magaszine.Pop().Get();
                var left = c._magaszine.Pop().Get();
                c._magaszine.Push(left == right);
            },
            (c) =>
            {
                var right = c._magaszine.Pop().Get();
                var left = c._magaszine.Pop().Get();
                c._magaszine.Push(left != right);
            },
            (c) =>
            {
                var right = c._magaszine.Pop().Get();
                var left = c._magaszine.Pop().Get();
                c._magaszine.Push(left > right);
            },
            (c) =>
            {
                var right = c._magaszine.Pop().Get();
                var left = c._magaszine.Pop().Get();
                c._magaszine.Push(left < right);
            },
            (c) =>
            {
                var right = c._magaszine.Pop().Get();
                var left = c._magaszine.Pop().Get();
                c._magaszine.Push(left >= right);
            },
            (c) =>
            {
                var right = c._magaszine.Pop().Get();
                var left = c._magaszine.Pop().Get();
                c._magaszine.Push(left <= right);
            },
            (c) =>
            {
                var right = c._magaszine.Pop().Get();
                var left = c._magaszine.Pop().Get();
                c._magaszine.Push(left | right);
            },
            (c) =>
            {
                var right = c._magaszine.Pop().Get();
                var left = c._magaszine.Pop().Get();
                c._magaszine.Push(left & right);
            },
            (c) =>
            {
                var right = c._magaszine.Pop().Get();
                c._magaszine.Push(!right);
            },
            (c) =>
            {
                var lValue = c._magaszine.Pop();
                var d = c._ReadDouble();
                lValue.Set(Elem.MakeValue(d));
            },
            (c) =>
            {
                var right = c._magaszine.Pop().Get();
                Console.Write($"{right} ");
            },
            (c) =>
            {
                Console.WriteLine();
            },
            (c) =>
            {
                var rigth = c._magaszine.Pop().Get();
                var left = c._magaszine.Pop().Get();
                c._magaszine.Push(left[rigth]);
            },
            (c) =>
            {
                var n = c._magaszine.Pop().Get();
                c._magaszine.Push(Elem.MakeArray(n));
            },
            (c) => c._magaszine.Push(Elem.MakeArg()),
            (c) => {
                Stack<string> names = new Stack<string>();
                Elem elem;
                while (true)
                {
                    elem = c._magaszine.Pop();
                    if (elem.Type == Elem.ElemType.Arg)
                    {
                        elem = c._magaszine.Pop();
                        break;
                    }
                    names.Push(elem.Name);
                }
                elem.AddFunction(new Function(){Names = names.ToArray(), m = c._k + 3});
            },
            (c) => c._magaszine.Push(Elem.MakeArg()),
            (c) =>
            {
                Stack<Elem> args = new Stack<Elem>();
                Elem elem;
                while (true)
                {
                    elem = c._magaszine.Pop();
                    if (elem.Type == Elem.ElemType.Arg)
                    {
                        elem = c._magaszine.Pop();
                        break;
                    }
                    args.Push(elem.Get());
                }

                var fun = elem.GetFunction();
                switch (fun.Names.Length.CompareTo(args.Count))
                {
                    case 1:
                        throw new Exception($"Не хватает аргументов для вызова функции {elem.Name}");
                    case -1:
                        throw new Exception($"Переданы лишние аргументы при вызове функции {elem.Name}");
                }

                var callStackElem = new CallStackElem(c.CurrentScope, c._k + 1);
                c._callStack.Push(callStackElem);
                for (int i = 0; i < fun.Names.Length; i++)
                {
                    var e = args.Pop();
                    Elem.MakeIdentifier(fun.Names[i], c.CurrentScope)
                        .AddVariable(e);
                }

                c._k = fun.m - 1;
            },
            (c) =>
            {
                var identifier = c._magaszine.Peek();
                identifier.AddVariable(Elem.MakeValue(0));
            },
            (c) => c._magaszine.Pop(),
            (c) =>
            {
                var callStackElem = c._callStack.Peek();
                var newsScope = new Scope(callStackElem.Scope);
                callStackElem.Scope = newsScope;
            },
            (c) =>
            {
                var callStackElem = c._callStack.Peek();
                var previousScope = callStackElem.Scope.Previous;
                callStackElem.Scope = previousScope;
            },
            (c) =>
            {
                var elem = c._magaszine.Pop().Get();
                c._k = c._callStack.Pop().ReturnTo - 1;
                c._magaszine.Push(elem);
            },
            (c) =>
            {
                c._k = c._callStack.Pop().ReturnTo - 1;
                c._magaszine.Push(Elem.MakeValue(0));
            },
            (c) =>
            {
                var m = c._magaszine.Pop().Get();
                var elem = c._magaszine.Pop().Get();
                if (elem.Compare(Elem.MakeValue(0)) == 0) c._k = (int) m.X - 1;
            },
            (c) =>
            {
                var m = c._magaszine.Pop().Get();
                c._k = (int) m.X - 1;
            },
        };
    }
}