using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiglusoft.IAM.Domain.Specifications.Abstracts
{
    public abstract class Specification<T>
    {
        public abstract bool IsSatisfiedBy(T entity);
        public abstract string Message { get; }
    }

    public class AndSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        public AndSpecification(Specification<T> left, Specification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override bool IsSatisfiedBy(T entity)
        {
            return _left.IsSatisfiedBy(entity) && _right.IsSatisfiedBy(entity);
        }

        public override string Message
        {
            get
            {
                if (!_left.IsSatisfiedBy(default(T)))
                    return _left.Message;
                if (!_right.IsSatisfiedBy(default(T)))
                    return _right.Message;
                return string.Empty;
            }
        }
    }

    public class OrSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        public OrSpecification(Specification<T> left, Specification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override bool IsSatisfiedBy(T entity)
        {
            return _left.IsSatisfiedBy(entity) || _right.IsSatisfiedBy(entity);
        }

        public override string Message
        {
            get
            {
                return $"{_left.Message} or {_right.Message}";
            }
        }
    }

    public class NotSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _specification;

        public NotSpecification(Specification<T> specification)
        {
            _specification = specification;
        }

        public override bool IsSatisfiedBy(T entity)
        {
            return !_specification.IsSatisfiedBy(entity);
        }

        public override string Message
        {
            get
            {
                return $"Not {_specification.Message}";
            }
        }
    }

}
