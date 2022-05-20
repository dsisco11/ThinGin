using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using ThinGin.Core.Console.Commands.Selectors.Expressions;
using ThinGin.Core.Console.Parsing;

namespace ThinGin.Core.Console.Commands.Selectors
{
    /*
     * == Selector types ==
     * - Item Single: @item
     * - Property: #player:0@property_name
     * - Property: #player:[:10]@property_name
     * - Property: #player:[:10]@status.health
     * - Property: @item:property_name
     * - Property: @item:status.health
     * - Item Array: @[item1,item2,item3]
     * - Filter: @($0.status.health > 0)
     * - Logical: @{ @item1.status.hp > @item2.status.hp }
     * 
     * == EXAMPLES ==
     * set_position @1924839857123980 69.5 420.0 17.1
     * set_position #object:1924839857123980 69.5 420.0 17.1
     * set #object:1924839857123980:position 69.5 420.0 17.1
     * set_position #player:1 0 0 0
     * set_data #player:0 status.health 999.0
     * get_data #player:0 status.health 999.0
     * 
     * set_data @player:0:status.health 999.0
     * set_data #player:[:10]:status.health 999.0
     * get_data #player:0@status.health 999.0
     * 
     * list #player:[1:100]@status.health
     * list @#player:[1:100]:status.health
     * list @{get_data #player:<0,10> status.health}
     */

    /// <summary>
    /// 
    /// </summary>
    public class Selector
    {
        #region Values
        private readonly List<Expression> Items;
        private Expression Body;
        #endregion

        #region Constructors
        public Selector(IList<Expression> items)
        {
            Items = new List<Expression>(items);
            Body = Expression.Block(Items);
            Optimize();
        }
        #endregion

        private void Optimize()
        {
            while (Body.CanReduce)
            {
                var reduced = Body.ReduceAndCheck();
                if (reduced is object)
                {
                    Body = reduced;
                }
            }
        }

        public Type Get_Return_Type()
        {
            return Body.Type;
        }

        #region Apply
        public virtual IEnumerable<object> Execute(ConsoleContext Context)
        {
            var ctxParam = Expression.Parameter(typeof(ConsoleContext), nameof(Context));
            var le = Expression.Lambda(Body, ctxParam);
            var compiled = le.Compile();
            var res = compiled.DynamicInvoke(Context);

            if (res is null)
                return null;
            else if (res is IEnumerable<object> eRes)
                return eRes;
            else
                return new[] { res };
        }

        //public virtual IEnumerable<object> Execute(ConsoleContext Context, IEnumerable<object> DataSet)
        //{
        //}
        #endregion

        #region Compiling
        public static Selector From(CSelectorToken selectorToken)
        {
            var Parser = new ExpressionParser(selectorToken.Values);
            var code = Parser.Parse();
            return new Selector(code);
        }
        #endregion
    }
}
