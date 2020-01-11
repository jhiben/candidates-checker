function check(element) {
  console.log('chek');

  const firstName = 'Sabrine';
  const lastName = 'Hadji';

  if (
    element.textContent &&
    element.textContent.match(new RegExp(firstName), 'i') &&
    element.textContent.match(new RegExp(lastName, 'i'))
  ) {
    element.style.color = 'red';
    element.innerHTML +=
      ' <small><i>(contacted on 11/01/2020 by Ginoto)</i></small>';
  }
}

const _PROFILE_MATCH = '.pv-top-card-v3--list > li:first-child';

function profileMatch(node) {
  return node.matches(_PROFILE_MATCH);
}

const _SEARCH_RESULT_MATCH = '.name.actor-name';

function searchResultsMatch(node) {
  return node.matches(_SEARCH_RESULT_MATCH);
}

const _SEARCH_POPUP_RESULT_MATCH =
  '.basic-typeahead__triggered-content.search-global-typeahead__content.search-box_focus .typeahead-suggestion .search-typeahead-v2__hit-info > span';

function searchPopupResultMatch(node) {
  return node.matches(_SEARCH_POPUP_RESULT_MATCH);
}

const _SEARCH_RECENT_MATCH =
  '.search-typeahead-v2__history-list-carousel > li .typeahead-suggestion--carousel-item-text';

function searchRecentMatch(node) {
  return node.matches(_SEARCH_RECENT_MATCH);
}

function match(node) {
  return (
    profileMatch(node) ||
    searchResultsMatch(node) ||
    searchPopupResultMatch(node) ||
    searchRecentMatch(node)
  );
}

var observer = new MutationObserver(mutations => {
  mutations.forEach(mutation => {
    for (let i = 0; i < mutation.addedNodes.length; i++) {
      const node = mutation.addedNodes[i];
      if (node.nodeType === Node.ELEMENT_NODE && match(node)) {
        check(node);
      }
    }
  });
});

observer.observe(document, { childList: true, subtree: true });

document.querySelectorAll(_PROFILE_MATCH).forEach(check);
document.querySelectorAll(_SEARCH_RESULT_MATCH).forEach(check);
document.querySelectorAll(_SEARCH_POPUP_RESULT_MATCH).forEach(check);
document.querySelectorAll(_SEARCH_RECENT_MATCH).forEach(check);
