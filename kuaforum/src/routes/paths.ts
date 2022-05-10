// ----------------------------------------------------------------------

function path(root: string, sublink: string) {
  return `${root}${sublink}`;
}

const ROOTS_AUTH = '/auth';
const ROOTS_DASHBOARD = '/';

// ----------------------------------------------------------------------

export const PATH_AUTH = {
  root: ROOTS_AUTH,
  login: path(ROOTS_AUTH, '/login'),
  loginUnprotected: path(ROOTS_AUTH, '/login-unprotected'),
  register: path(ROOTS_AUTH, '/register'),
  registerUnprotected: path(ROOTS_AUTH, '/register-unprotected'),
  resetPassword: path(ROOTS_AUTH, '/reset-password'),
  verify: path(ROOTS_AUTH, '/verify'),
};

export const PATH_PAGE = {
  comingSoon: '/coming-soon',
  maintenance: '/maintenance',
  pricing: '/pricing',
  payment: '/payment',
  about: '/about-us',
  contact: '/contact-us',
  faqs: '/faqs',
  page404: '/404',
  page500: '/500',
  components: '/components',
};

export const PATH_DASHBOARD = {
  root: '/home',
  raw:{
      root: '/raw',
      rawMaterials: '/raw/materials',
      createMaterial:'/raw/materials/create',
      categories:'/raw/categories',
      createCategory:'/raw/categories/create',
  },
  mattress:{
    create:'/mattress/create',
    created:'/mattress/created',
    types:'/mattress/types',
    stage:'/mattress/stage',
  },
  order:{
    root:'/order',
    createOrder:'/order/create',
    orders:'/order/orders',
  },
  general:{
    expense:'/general/expense',
    settings:'/general/settings'
  },
  production:{
    recipe: '/production/recipe',
    proforma: 'production/proforma'
  },
  customer:{
    customers: '/customers'
  }
};

export const PATH_DOCS = 'https://docs-minimals.vercel.app/introduction';
