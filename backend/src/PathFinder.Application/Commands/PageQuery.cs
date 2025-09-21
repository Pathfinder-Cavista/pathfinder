namespace PathFinder.Application.Commands
{
    public class PageQuery
    {
        private int _minPage = 1;
        private int _minSize = 5;
        private int _maxSize = 50;

        public int Page 
        {
            get => _minPage; 
            set => _minPage = value < _minPage ? _minPage : value;
        }
        public int Size 
        {
            get => _maxSize;
            set => _maxSize = value > _maxSize ? _maxSize : 
                value < _minSize ? _minSize : value;
        }
    }
}
