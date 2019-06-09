const selectionSort = require('./selection_sort').default;

test('handles empty array', () => {
  const items = [];
  const sortedItems = selectionSort(items);

  expect(sortedItems).toEqual([]);
});

test('sorts items', () => {
  const items = [5, 1, 6, 2, 7, 4, 3];
  const sortedItems = selectionSort(items);

  expect(sortedItems).toEqual([1, 2, 3, 4, 5, 6, 7]);
});

test('sorts items with duplicates', () => {
  const items = [1, 1, 4, 3, 4, 1];
  const sortedItems = selectionSort(items);

  expect(sortedItems).toEqual([1, 1, 1, 3, 4, 4]);
});
