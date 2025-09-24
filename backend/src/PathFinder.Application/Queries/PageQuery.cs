namespace PathFinder.Application.Queries
{
    public class PageQuery
    {
        private int _minPage = 1;
        private int _minSize = 10;
        private int _maxSize = 50;

        public int Page 
        {
            get => _minPage; 
            set => _minPage = value < _minPage ? _minPage : value;
        }
        public int Size 
        {
            get => _minSize;
            set => _minSize = value < _minSize ? _minSize : 
                value > _maxSize ? _maxSize : value;
        }
    }
}
