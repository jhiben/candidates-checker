const _PROFILE_MATCH = [
  '.pv-top-card-v3--list > li.t-24',
  '.pv-top-card--list > li.t-24',
  '.profile-info > h1.searchable',
].join(',');

const _SEARCH_RESULT_MATCH = [
  '.name.actor-name',
  '.search-result-profile-link',
].join(',');

const _SEARCH_POPUP_RESULT_MATCH =
  '.basic-typeahead__triggered-content.search-global-typeahead__content.search-box_focus .typeahead-suggestion .search-typeahead-v2__hit-info > span';

const _SEARCH_RECENT_MATCH =
  '.search-typeahead-v2__history-list-carousel > li .typeahead-suggestion--carousel-item-text';

const _PRO = [
  '.lockup__content-title .artdeco-entity-lockup__title > a',
  '.lockup__content-title div.artdeco-entity-lockup__title',
  '.system-search-results__result-row .artdeco-entity-lockup__title > span',
].join(',');

const _ALL = [
  _PROFILE_MATCH,
  _SEARCH_RESULT_MATCH,
  _SEARCH_POPUP_RESULT_MATCH,
  _SEARCH_RECENT_MATCH,
  _PRO,
].join(',');

checker(_ALL);
